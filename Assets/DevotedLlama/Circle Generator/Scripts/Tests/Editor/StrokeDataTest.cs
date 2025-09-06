using NUnit.Framework;

namespace DevotedLlama.CircleGenerator.Tests {

    public class StrokeDataTest {
        [Test]
        public void TestStrokeArgException() {
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate {
                StrokeData SD = new StrokeData(-10, true);
            });
        }
    }
}
