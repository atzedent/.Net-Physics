using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using IMaverick.Physics.Extensions;
using System.Xml.Serialization;
using System.Collections;

namespace IMaverick.Physics.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [FixtureTearDown]
        public void Cleanup()
        {
            var files = new List<string> { "DataFile.dat", "Data.xml" };

            files.ForEach(file => { if (File.Exists(file)) File.Delete(file); });
        }

        [Test(Order = 0)]
        public void TestSerialize()
        {
            FileStream fs = new FileStream("DataFile.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            var length = 2d.m();
            formatter.Serialize(fs, length);

            fs.Close();
        }

        [Test(Order = 1)]
        public void TestDeserialize()
        {
            PhysicalValue<Length> length = default(PhysicalValue<Length>);

            FileStream fs = new FileStream("DataFile.dat", FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            length = (PhysicalValue<Length>)formatter.Deserialize(fs);
            fs.Close();

            Assert.AreEqual(2d.m(), length);
        }

        [Test(Order = 2)]
        public void TestXmlSerialize()
        {
            var length = 2d.m();
            var serializer = new XmlSerializer(typeof(PhysicalValue<Length>));
            TextWriter writer = new StreamWriter("Data.xml");
            serializer.Serialize(writer, length);
            writer.Close();
        }

        [Test(Order = 3)]
        public void TestXmlDeserialize()
        {
            var serializer = new XmlSerializer(typeof(PhysicalValue<Length>));
            var reader = new StreamReader("Data.xml");
            var length = (PhysicalValue<Length>)serializer.Deserialize(reader);
            reader.Close();

            Assert.AreEqual(2d.m(), length);
        }

        [Test]
        [XmlData("/Parameter", FilePath = "Parameter.xml")]
        public void ParameterizedTest(
            [Bind("Tests/ParameterizedTest/Length/PhysicalValue")] PhysicalValue<Length> length,
            [Bind("Tests/ParameterizedTest/Temperature/PhysicalValue")] PhysicalValue<Temperature> temperature,
            [Bind("Globals/Length/PhysicalValue")] PhysicalValue<Length> globalLength)
        {
            Assert.AreEqual(0.02.m(), length);
            Assert.AreEqual(length, globalLength);
            Assert.AreEqual(30d.Celsius(), temperature);
        }
    }
}
