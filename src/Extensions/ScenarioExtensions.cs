using System;
using Xbehave.Sdk;
using Xbehave;

namespace AutomationLibrary.Extensions
{
    public static class ScenarioExtensions
    {
        public static IStepBuilder Do(this string text, Action body)
        {
            return text.x(body);
        }
    }
}