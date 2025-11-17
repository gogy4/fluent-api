using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.HomeWork.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [Test]
        public void Demo()
        {
            var father = new Person {Name = "Alex", Age = 44, Height = 185};
            var son = new Person{Name = "Son", Age = 2, Parent = father};
            father.Child = son;

            var printer = ObjectPrinter
                .For<Person>()
                .Exclude<int>()
                .SetNumericCulture(CultureInfo.InvariantCulture)
                .Serialize<int>(x=>$"Возраст: {x}")
                .Serialize(x=>x.Name, x=>$"Имя: {x}")
                .Trim(x=>x.Name, 2)
                .Exclude(x=>x.Height);
            //1. Исключить из сериализации свойства определенного типа
            //2. Указать альтернативный способ сериализации для определенного типа
            //3. Для числовых типов указать культуру
            //4. Настроить сериализацию конкретного свойства
            //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
            //6. Исключить из сериализации конкретного свойства
            
            var actual = printer.PrintToString(father);
            var expected =
                "Person\r\n\tName = Al\n\tChild = Person\r\n\t\tName = So\n\t\tParent = <see Person Al>";
            actual.Should().Be(expected);

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию        
            //8. ...с конфигурированием
        }
    }
}