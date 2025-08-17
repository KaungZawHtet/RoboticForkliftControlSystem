using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using RoboticForkliftControlSystem.Api.Dtos;
using RoboticForkliftControlSystem.Api.Services;

namespace RoboticForkliftControlSystem.Test
{
    [TestFixture]
    public class MovementServiceTests
    {
        private MovementService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new MovementService();
        }

        [Test]
        public void ParseMovementCommand_ValidMixedCommands_ReturnsCommandsAndValid()
        {
            // Arrange
            var input = "F10R90L90B5";

            // Act
            var result = _sut.ParseMovementCommand(input);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Commands, Has.Count.EqualTo(4));

            Assert.Multiple(() =>
            {
                Assert.That(result.Commands[0].Action, Is.EqualTo("F"));
                Assert.That(result.Commands[0].Value, Is.EqualTo(10));
                Assert.That(
                    result.Commands[0].Description,
                    Is.EqualTo("Move Forward by 10 metres.")
                );

                Assert.That(result.Commands[1].Action, Is.EqualTo("R"));
                Assert.That(result.Commands[1].Value, Is.EqualTo(90));
                Assert.That(
                    result.Commands[1].Description,
                    Is.EqualTo("Turn Right by 90 degrees.")
                );

                Assert.That(result.Commands[2].Action, Is.EqualTo("L"));
                Assert.That(result.Commands[2].Value, Is.EqualTo(90));
                Assert.That(result.Commands[2].Description, Is.EqualTo("Turn Left by 90 degrees."));

                Assert.That(result.Commands[3].Action, Is.EqualTo("B"));
                Assert.That(result.Commands[3].Value, Is.EqualTo(5));
                Assert.That(
                    result.Commands[3].Description,
                    Is.EqualTo("Move Backward by 5 metres.")
                );
            });
        }

        [Test]
        public void ParseMovementCommand_IsCaseInsensitive()
        {
            var result = _sut.ParseMovementCommand("f10r90");

            Assert.That(result.IsValid, Is.True);
            Assert.That(
                result.Commands.Select(c => c.Action),
                Is.EqualTo(new[] { "F", "R" }).AsCollection
            );
            Assert.That(
                result.Commands.Select(c => c.Value),
                Is.EqualTo(new[] { 10, 90 }).AsCollection
            );
        }

        [Test]
        public void ParseMovementCommand_EmptyInput_IsInvalid()
        {
            var result = _sut.ParseMovementCommand("");

            Assert.That(result.IsValid, Is.False);
            Assert.That(string.Join(" | ", result.Errors), Does.Contain("Command cannot be empty"));
            Assert.That(result.Commands, Is.Empty);
        }

        [Test]
        public void ParseMovementCommand_NoValidTokens_IsInvalid()
        {
            var result = _sut.ParseMovementCommand("XYZ");

            Assert.That(result.IsValid, Is.False);
            Assert.That(
                string.Join(" | ", result.Errors),
                Does.Contain("No valid commands found in input")
            );
            Assert.That(result.Commands, Is.Empty);
        }

        [Test]
        public void ParseMovementCommand_JunkBetweenTokens_FlagsErrorButKeepsValidParts()
        {
            var result = _sut.ParseMovementCommand("F10X5");

            // F10 parsed, 'X5' is junk -> error present
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Commands, Has.Count.EqualTo(1));
            AssertHasErrorContaining(result, "unexpected characters");
        }

        [Test]
        public void ParseMovementCommand_SpacesBetweenTokens_FlagsErrorButKeepsValidParts()
        {
            var result = _sut.ParseMovementCommand("F10 R90");

            Assert.That(result.IsValid, Is.False); // space is treated as junk
            Assert.That(result.Commands, Has.Count.EqualTo(2)); // both tokens parsed
            AssertHasErrorContaining(result, "unexpected characters");
        }

        // --- helpers ---
        private static void AssertHasErrorContaining(MovementResult result, string part)
        {
            var all = string.Join(" | ", result.Errors).ToLowerInvariant();
            Assert.That(all, Does.Contain(part.ToLowerInvariant()));
        }
    }
}
