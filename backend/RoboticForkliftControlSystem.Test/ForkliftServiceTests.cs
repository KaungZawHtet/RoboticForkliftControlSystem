using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RoboticForkliftControlSystem.Api.Data;
using RoboticForkliftControlSystem.Api.Entities;
using RoboticForkliftControlSystem.Api.Services;

namespace RoboticForkliftControlSystem.Test
{
    public class ForkliftServiceTests
    {
        private AppDbContext _db = null!;
        private ForkliftService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _db.Database.EnsureCreated();

            _sut = new ForkliftService(_db);
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        [Test]
        public async Task GetAllForkliftsAsync_ReturnsSeededData()
        {
            // Arrange
            _db.Forklifts.AddRange(
                new Forklift
                {
                    Name = "A1",
                    ModelNumber = "M-001",
                    ManufacturingDate = new DateTime(2020, 1, 1),
                },
                new Forklift
                {
                    Name = "B2",
                    ModelNumber = "M-002",
                    ManufacturingDate = new DateTime(2021, 2, 2),
                }
            );
            await _db.SaveChangesAsync();

            // Act
            var rows = await _sut.GetAllForkliftsAsync();

            // Assert
            Assert.That(rows, Has.Count.EqualTo(2));
            Assert.That(rows.Select(r => r.Name), Is.EquivalentTo(new[] { "A1", "B2" }));
        }

        [Test]
        public async Task SaveForkliftsAsync_PersistsRows()
        {
            // Arrange
            var toSave = new List<Forklift>
            {
                new()
                {
                    Name = "X",
                    ModelNumber = "MX",
                    ManufacturingDate = new DateTime(2024, 3, 10),
                },
                new()
                {
                    Name = "Y",
                    ModelNumber = "MY",
                    ManufacturingDate = new DateTime(2023, 11, 5),
                },
            };

            // Act
            await _sut.SaveForkliftsAsync(toSave);

            // Assert
            var all = await _db.Forklifts.OrderBy(f => f.Name).ToListAsync();
            Assert.That(all, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(all[0].Name, Is.EqualTo("X"));
                Assert.That(all[0].ModelNumber, Is.EqualTo("MX"));
                Assert.That(all[0].ManufacturingDate, Is.EqualTo(new DateTime(2024, 3, 10)));

                Assert.That(all[1].Name, Is.EqualTo("Y"));
                Assert.That(all[1].ModelNumber, Is.EqualTo("MY"));
                Assert.That(all[1].ManufacturingDate, Is.EqualTo(new DateTime(2023, 11, 5)));
            });
        }

        [Test]
        public async Task ImportForkliftsFromCsvAsync_EmptyOrHeaderOnly_ReturnsEmpty()
        {
            using var empty = MakeStream("");
            using var headerOnly = MakeStream("Name,ModelNumber,ManufacturingDate\n");

            var r1 = await _sut.ImportForkliftsFromCsvAsync(empty);
            var r2 = await _sut.ImportForkliftsFromCsvAsync(headerOnly);

            Assert.That(r1, Is.Empty);
            Assert.That(r2, Is.Empty);
        }

        [Test]
        public async Task ImportForkliftsFromJsonAsync_DeserializesCaseInsensitive()
        {
            // Arrange (mixed casing and extra fields)
            var json = """
                [
                  { "name": "Foxtrot", "modelNumber": "F-600", "manufacturingDate": "2021-07-20", "extra":"x" },
                  { "Name": "Golf", "ModelNumber": "G-700", "ManufacturingDate": "2022-12-01" }
                ]
                """;
            using var stream = MakeStream(json);

            // Act
            var rows = await _sut.ImportForkliftsFromJsonAsync(stream);

            // Assert
            Assert.That(rows, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                var f = rows.Single(r => r.Name == "Foxtrot");
                Assert.That(f.ModelNumber, Is.EqualTo("F-600"));
                Assert.That(f.ManufacturingDate, Is.EqualTo(new DateTime(2021, 7, 20)));

                var g = rows.Single(r => r.Name == "Golf");
                Assert.That(g.ModelNumber, Is.EqualTo("G-700"));
                Assert.That(g.ManufacturingDate, Is.EqualTo(new DateTime(2022, 12, 1)));
            });
        }

        [Test]
        public async Task ImportForkliftsFromJsonAsync_NullOrEmptyArray_ReturnsEmpty()
        {
            using var nullJson = MakeStream("null");
            using var emptyArray = MakeStream("[]");

            var r1 = await _sut.ImportForkliftsFromJsonAsync(nullJson);
            var r2 = await _sut.ImportForkliftsFromJsonAsync(emptyArray);

            Assert.That(r1, Is.Empty);
            Assert.That(r2, Is.Empty);
        }

        // Utility: make a readable stream
        private static MemoryStream MakeStream(string content)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }
    }
}