using System.Collections.Concurrent;
using System.IO.Compression;
using System.Reflection;
using System.Xml.XPath;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Schematron.Helpers
{
    /// <summary>
    /// Donne accès aux ressources textuelles embarquées compressées : Schematron (.sch), listes de codes (_codedb.xml) et XSD.
    /// Ces fichiers sont très redondants entre niveaux de conformité : ils sont embarqués dans
    /// un unique zip (généré à la compilation, cf. cible PackCompressedResources du csproj)
    /// et décompressés à la demande, une seule fois chacun.
    /// </summary>
    internal static class CompressedResources
    {
        private const string ArchiveResourceName = "Securibox.FacturX.compress-resources.zip";

        private static readonly ConcurrentDictionary<string, Lazy<byte[]?>> Entries =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, Lazy<IReadOnlyList<byte[]>>> XsdFiles =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, Lazy<XPathDocument?>> CodeDbs =
            new(StringComparer.OrdinalIgnoreCase);

        public static byte[] GetSchema(FacturXConformanceLevelType conformanceLevel)
        {
            string fileName;
            if (conformanceLevel.Id == FacturXConformanceLevelType.Minimum.Id)
            {
                fileName = "FACTUR-X_MINIMUM.sch";
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.BasicWL.Id)
            {
                fileName = "FACTUR-X_BASIC-WL.sch";
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Basic.Id)
            {
                fileName = "FACTUR-X_BASIC.sch";
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.EN16931.Id)
            {
                fileName = "FACTUR-X_EN16931.sch";
            }
            else if (conformanceLevel.Id == FacturXConformanceLevelType.Extended.Id)
            {
                fileName = "FACTUR-X_EXTENDED.sch";
            }
            else
            {
                throw new ArgumentException("Could not determine FacturXConformanceLevelType");
            }

            return GetEntry(fileName) ?? throw new FileNotFoundException($"Schematron resource not found: {fileName}");
        }

        /// <summary>
        /// Retourne la liste de codes (parsée une seule fois) ou null si elle n'existe pas.
        /// </summary>
        /// <param name="name">Nom sans extension, ex. "FACTUR-X_BASIC_codedb".</param>
        public static XPathDocument? GetCodeDb(string name)
        {
            return CodeDbs
                .GetOrAdd(
                    name,
                    key => new Lazy<XPathDocument?>(() =>
                    {
                        var bytes = GetEntry(key + ".xml");
                        if (bytes == null)
                        {
                            return null;
                        }

                        using var stream = new MemoryStream(bytes, writable: false);
                        return new XPathDocument(stream);
                    })
                )
                .Value;
        }

        /// <summary>
        /// Retourne le contenu de tous les XSD du niveau de conformité (dossier Xsd/FacturX/{niveau}/).
        /// </summary>
        public static IReadOnlyList<byte[]> GetXsdFiles(FacturXConformanceLevelType conformanceLevel)
        {
            return XsdFiles
                .GetOrAdd(
                    conformanceLevel.Name,
                    levelName => new Lazy<IReadOnlyList<byte[]>>(() => ReadXsdFiles(levelName))
                )
                .Value;
        }

        private static IReadOnlyList<byte[]> ReadXsdFiles(string levelName)
        {
            var prefix = $"Xsd/FacturX/{levelName}/";
            var files = new List<byte[]>();

            using var archiveStream = OpenArchiveStream();
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                // Selon l'outil qui a créé le zip, le séparateur peut être '\' ou '/'.
                var name = entry.FullName.Replace('\\', '/');
                if (
                    name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    && name.EndsWith(".xsd", StringComparison.OrdinalIgnoreCase)
                )
                {
                    files.Add(ReadAll(entry));
                }
            }

            return files;
        }

        private static byte[]? GetEntry(string entryName)
        {
            return Entries.GetOrAdd(entryName, key => new Lazy<byte[]?>(() => ReadEntry(key))).Value;
        }

        private static Stream OpenArchiveStream()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(ArchiveResourceName)
                   ?? throw new FileNotFoundException(
                       $"Embedded resource not found: {ArchiveResourceName}"
                   );
        }

        private static byte[]? ReadEntry(string entryName)
        {
            using var archiveStream = OpenArchiveStream();
            using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);

            var entry = archive.GetEntry(entryName);
            return entry == null ? null : ReadAll(entry);
        }

        private static byte[] ReadAll(ZipArchiveEntry entry)
        {
            using var entryStream = entry.Open();
            using var buffer = new MemoryStream((int)entry.Length);
            entryStream.CopyTo(buffer);
            return buffer.ToArray();
        }
    }
}
