using System.Collections.Concurrent;
using System.IO.Compression;
using System.Reflection;
using System.Xml.XPath;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Schematron.Helpers
{
    /// <summary>
    /// Provides access to compressed embedded textual resources: Schematron (.sch), code lists (_codedb.xml), and XSDs.
    /// These files are highly redundant across compliance levels: they are embedded in
    /// a single zip file (generated at compile time, see the PackCompressedResources target in the csproj)
    /// and decompressed on demand, exactly once each.
    /// </summary>
    internal static class CompressedResources
    {
        private const string ArchiveResourceName = "Securibox.FacturX.compress-resources.zip";

        private static readonly ConcurrentDictionary<string, Lazy<byte[]?>> Entries = new(
            StringComparer.OrdinalIgnoreCase
        );

        private static readonly ConcurrentDictionary<string, Lazy<IReadOnlyList<byte[]>>> XsdFiles =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, Lazy<XPathDocument?>> CodeDbs = new(
            StringComparer.OrdinalIgnoreCase
        );

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

            return GetEntry(fileName)
                ?? throw new FileNotFoundException($"Schematron resource not found: {fileName}");
        }

        /// <summary>
        /// Returns the code list (parsed only once), or null if it does not exist.
        /// </summary>
        /// <param name="name">Name without extension, e.g. "FACTUR-X_BASIC_codedb".</param>
        public static XPathDocument? GetCodeDb(string name)
        {
            return CodeDbs
                .GetOrAdd(name, key => new Lazy<XPathDocument?>(() =>
                {
                    var bytes = GetEntry(key + ".xml");
                    if (bytes == null)
                    {
                        return null;
                    }

                    using var stream = new MemoryStream(bytes, writable: false);
                    return new XPathDocument(stream);
                }))
                .Value;
        }

        /// <summary>
        /// Returns the content of all the XSD files of the conformance level (folder Xsd/FacturX/{level}/).
        /// </summary>
        public static IReadOnlyList<byte[]> GetXsdFiles(
            FacturXConformanceLevelType conformanceLevel
        )
        {
            return XsdFiles
                .GetOrAdd(conformanceLevel.Name, levelName => new Lazy<IReadOnlyList<byte[]>>(() =>
                    ReadXsdFiles(levelName)
                ))
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
                // Depending on the tool that created the zip, the separator can be '\' or '/'.
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
            return Entries
                .GetOrAdd(entryName, key => new Lazy<byte[]?>(() => ReadEntry(key)))
                .Value;
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
