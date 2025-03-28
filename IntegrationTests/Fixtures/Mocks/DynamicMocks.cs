using System.Reflection;
using Bogus;

namespace IntegrationTests.Fixtures.Mocks
{
    public static class MockGenerator
    {
        private static readonly Faker Faker = new();

        public static T GenerateMock<T>()
        {
            return (T)GenerateMock(typeof(T));
        }

        private static object GenerateMock(Type type)
        {
            return type switch
            {
                _ when type == typeof(bool) => Faker.Random.Bool(),
                _ when type == typeof(char) => Faker.Random.Char('a', 'z'),
                _ when type == typeof(sbyte) => Faker.Random.SByte(sbyte.MinValue, sbyte.MaxValue),
                _ when type == typeof(byte) => Faker.Random.Byte(byte.MinValue, byte.MaxValue),
                _ when type == typeof(short) => Faker.Random.Short(short.MinValue, short.MaxValue),
                _ when type == typeof(ushort) => Faker.Random.UShort(ushort.MinValue, ushort.MaxValue),
                _ when type == typeof(int) => Faker.Random.Int(int.MinValue, int.MaxValue),
                _ when type == typeof(uint) => Faker.Random.UInt(uint.MinValue, uint.MaxValue),
                _ when type == typeof(long) => Faker.Random.Long(long.MinValue, long.MaxValue),
                _ when type == typeof(ulong) => Faker.Random.ULong(0, ulong.MaxValue),
                _ when type == typeof(float) => Faker.Random.Float(float.MinValue, float.MaxValue),
                _ when type == typeof(double) => Faker.Random.Double(double.MinValue, double.MaxValue),
                _ when type == typeof(string) => Faker.Lorem.Word(),
                _ when type == typeof(Guid) => Guid.NewGuid(),
                _ => throw new Exception("Not available type")
            };
        }
    }
}
