using TwBlazor.Abstraction;

namespace TwBlazor.Tests.Abstraction;

public class ITwInputComponentTests
{
    private class TestComponent : ITwInputComponent
    {
        public string RootId { get; set; } = string.Empty;
        public string RootClass { get; set; } = string.Empty;
        public Dictionary<string, object> RootAttributes { get; set; } = [];
        public string Label { get; set; } = string.Empty;
        public string LabelId { get; set; } = string.Empty;
        public Dictionary<string, object> LabelAttributes { get; set; } = [];
        public string LabelClass { get; set; } = string.Empty;
        public bool ReadOnly { get; set; }
        public bool Disabled { get; set; }
        public bool Invalid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    [Fact]
    public void LabelClasses_DefaultImplementation_BuildsClassesFromLabelClass()
    {
        // Arrange
        ITwInputComponent component = new TestComponent { LabelClass = "text-blue-600" };

        // Act
        var classes = component.LabelClasses;

        // Assert
        Assert.Contains("text-blue-600", classes);
    }

    [Fact]
    public void LabelClasses_DefaultImplementation_HandlesEmptyLabelClass()
    {
        // Arrange
        ITwInputComponent component = new TestComponent();

        // Act
        var classes = component.LabelClasses;

        // Assert
        Assert.NotNull(classes);
    }

    [Fact]
    public async Task Close_DefaultImplementation_CompletesSuccessfully()
    {
        // Arrange
        ITwInputComponent component = new TestComponent();

        // Act
        var task = component.Close();
        await task;

        // Assert
        Assert.True(task.IsCompletedSuccessfully);
    }
}
