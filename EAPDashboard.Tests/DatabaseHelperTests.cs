using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using EAPDashboard.Models;
using FluentAssertions;
using Xunit;

namespace EAPDashboard.Tests
{
    public class DatabaseHelperTests
    {
        [Fact]
        public void InitializeDatabase_ShouldNotThrowException()
        {
            // Act
            Action act = () => DatabaseHelper.InitializeDatabase();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void GetMappedRtnCode_ShouldReturnDefaultOrMappedCode()
        {
            // Act
            string code = DatabaseHelper.GetMappedRtnCode("TrackIn", "001");

            // Assert
            code.Should().NotBeNull();
        }

        [Fact]
        public void GetPendingBindings_ShouldReturnListInstance()
        {
            // Act
            var bindings = DatabaseHelper.GetPendingBindings("EQ01");

            // Assert
            bindings.Should().NotBeNull();
        }
    }
}
