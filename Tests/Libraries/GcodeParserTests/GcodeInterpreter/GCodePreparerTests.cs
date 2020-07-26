using GcodeParser.GcodeInterpreter;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace GcodeParser.GcodeInterpreter.Tests
{
    [TestFixture()]
    public class GCodePreparerTests
    {
        [Test()]
        [TestCase("G2 X0.363 Y1.184 I-0.0642 J0.0021 F4")]
        public void FindTestValidString(string value)
        {
            var output = GCodePreparer.Find(value).Trim();

            Assert.IsTrue(output.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
        [Test()]
        [TestCase("(Use a .015 conical or flat mill for a cutter.....cuts .031\" deep)")]
        public void FindTestInvalidString(string value)
        {
            var output = GCodePreparer.Find(value).Trim();

            Assert.IsFalse(output.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
        [Test()]
        [TestCase("G3 X0.046 Y0.756 I0.3098 J0.0954 (Use a .015 conical or flat mill for a cutter.....cuts .031\" deep)")]
        public void FindTestStringWithComments(string value)
        {
            const string valid = "G3 X0.046 Y0.756 I0.3098 J0.0954";

            var output = GCodePreparer.Find(value).Trim();

            Assert.IsTrue(output.Equals(valid, StringComparison.OrdinalIgnoreCase));
        }
        [Test()]
        public void FindTestNullArgument()
        {
            var output = GCodePreparer.Find(null).Trim();

            Assert.IsTrue(string.IsNullOrEmpty(output));
        }

        [Test()]
        public void OpenFileAsyncTestValidFile()
        {
            var validFilePath = @".\TestFiles\valid.nc";
            var preparer = new GCodePreparer();

            Assert.DoesNotThrowAsync(async () => await preparer.OpenFileAsync(validFilePath, new Progress<float>()));
        }

        [Test()]
        public void OpenFileAsyncNotExistingFile()
        {
            var notexistNc = @".\notexist.nc";
            var preparer = new GCodePreparer();

            Assert.ThrowsAsync<FileNotFoundException>(async () => await preparer.OpenFileAsync(notexistNc, new Progress<float>()));
        }

        [Test()]
        public async Task PrepareStringsAsyncTest()
        {
            var validFilePath = @".\TestFiles\valid.nc";
            var preparer = new GCodePreparer();
            await preparer.OpenFileAsync(validFilePath, new Progress<float>());

            try
            {
                await preparer.PrepareStringsAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.Pass();
        }

        [Test()]
        public async Task PrepareStringsAsyncInvalidFileTest()
        {
            var validFilePath = @".\TestFiles\invalid.nc";
            var preparer = new GCodePreparer();
            await preparer.OpenFileAsync(validFilePath, new Progress<float>());

            try
            {
                await preparer.PrepareStringsAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            Assert.IsTrue(preparer.Strings.Count == 0);
        }
    }
}