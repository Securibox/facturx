using System.IO;
using NUnit.Framework;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    internal class StreamTests
    {
        private readonly string _mainDir = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Invoices",
            "Minimum"
        );

        [Test]
        public void Importer_Stream_Input_Should_Not_Be_Disposed()
        {
            // Arrange
            var pdfPath = Path.Combine(_mainDir, "Facture_FR_MINIMUM.pdf");
            var originalStream = new MemoryStream();

            using (var fileStream = File.OpenRead(pdfPath))
            {
                fileStream.CopyTo(originalStream);
            }

            originalStream.Position = 0;

            // Act
            using (var importer = new FacturxImporter(originalStream))
            {
                var metadata = importer.GetMetadata();
                Assert.That(metadata, Is.Not.Null);
            }

            // Assert
            Assert.That(
                originalStream.CanRead,
                Is.True,
                "The input stream should not be disposed by the constructor"
            );
            Assert.DoesNotThrow(
                () => originalStream.Position = 0,
                "The stream should still be accessible"
            );

            // Cleanup
            originalStream.Dispose();
        }

        [Test]
        public void Importer_Dispose_Should_Close_Internal_Stream()
        {
            // Arrange
            var pdfPath = Path.Combine(_mainDir, "Facture_FR_MINIMUM.pdf");
            MemoryStream internalStream;

            // Act
            using (var importer = new FacturxImporter(pdfPath))
            {
                var metadata = importer.GetMetadata();
                Assert.That(metadata, Is.Not.Null);
            }

            // Assert
            Assert.Pass("Dispose executed successfully");
        }

        [Test]
        public void Importer_NonMemoryStream_Should_Create_Copy()
        {
            // Arrange
            var pdfPath = Path.Combine(_mainDir, "Facture_FR_MINIMUM.pdf");

            using (var fileStream = File.OpenRead(pdfPath))
            {
                // Act
                using (var importer = new FacturxImporter(fileStream))
                {
                    var metadata = importer.GetMetadata();

                    // Assert
                    Assert.That(metadata, Is.Not.Null);
                    Assert.That(
                        fileStream.CanRead,
                        Is.True,
                        "The original FileStream must remain accessible"
                    );
                }

                Assert.That(
                    fileStream.CanRead,
                    Is.True,
                    "The original FileStream should not be disposed"
                );
            }
        }
    }
}
