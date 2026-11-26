using GermanTrainer.Core.Models;

namespace GermanTrainer.Tests;

public class CourseLessonTests
{
    [Fact] public void KeysIncludeLevel()
    {
        Assert.NotEqual(new CourseLesson("A1", 1).Key, new CourseLesson("A2", 1).Key);
        Assert.Equal("B1:12", new CourseLesson(" b1 ", 12).Key);
        Assert.Equal("Без уровня:2", new CourseLesson("", 2).Key);
    }
}
