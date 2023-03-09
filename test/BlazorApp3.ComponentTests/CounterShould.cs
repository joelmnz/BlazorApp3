using BlazorApp3.Pages;

// Example Blazor Component testing using BUnit : https://bunit.dev/

namespace BlazorApp3.ComponentTests;

public class CounterShould
{
	[Fact]
	public void RenderCorrectlyWithInitialZero()
	{
		using var ctx = new TestContext();

		var cut = ctx.RenderComponent<Counter>();
		
		// can match entire block
		// cut.MarkupMatches("""
		//       <h1>Counter</h1>
		//       <p role="status">Current count: 0</p>
		//       """);
		
		// or match just part
		cut.Find("p")
			.MarkupMatches("<p role=\"status\">Current count: 0</p>");
	}

	[Fact]
	public void IncrementCounterWhenButtonIsClicked()
	{
		using var ctx = new TestContext();
		var cut = ctx.RenderComponent<Counter>();
		cut.Find(cssSelector: "button")
			.Click();

		cut.Find(cssSelector: "p")
			.MarkupMatches("<p role=\"status\">Current count: 1</p>");
	}
}