#light 

namespace IMaverick.Physics.Tests

    open MbUnit.Framework 
    open IMaverick.Physics
    open IMaverick.Physics.Extensions 
    
    module Tests =

        [<Test>] 
        let TestInitialization() = 
            Assert.AreEqual(1.0.m(), 0.001.km())
            Assert.AreEqual(30.0.Celsius(), 86.0.Fahrenheit())
            Assert.AreEqual(1.0, 30.0.Celsius().DivideBy(86.0.Fahrenheit()))
            
        [<Test>]
        let TestEqualityOfElementsOfSameScale() = 
            let expected = [| for i in 1 .. 1000000 -> new PhysicalValue<Length>(System.Convert.ToDouble(i), Length.Meter )|]
            let actual = [| for i in 1 .. 1000000 -> new PhysicalValue<Length>(System.Convert.ToDouble(i), Length.Meter )|]
            Assert.AreElementsEqual(expected, actual)

        [<Test>]
        let TestEqualityOfElementsOfDifferentScale() = 
            let expected = [| for i in 1 .. 1000000 -> new PhysicalValue<Length>(System.Convert.ToDouble(i), Length.Meter )|]
            let actual = [| for i in 1 .. 1000000 -> (new PhysicalValue<Length>(System.Convert.ToDouble(i), Length.Meter )).ScaleTo(Length.Centimeter)|]
            Assert.AreElementsEqual(expected, actual)
    
        [<Test>]
        let TestCreationOfSequence() =
            let expected = Sequence.AddRange(1,1000000,Temperature.Celsius)
            let actual = expected.ScaleTo(Temperature.Kelvin)
            Assert.AreElementsEqual(expected,actual)