namespace Tests2
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int a = 1;
            int b = 2;
            int sum = 3;

            Assert.AreEqual(sum, 1 + 2);
        }
    }
}