using System.Windows;
using System.Windows.Media.Animation;

namespace Robot_Garage {
	public static class AnimationHelper {
		public static void PlayAnimation(FrameworkElement element, string animationKey) {
			if (element == null)
				return;
			var storyboard = element.TryFindResource(animationKey) as Storyboard;
			if (storyboard != null) {
				storyboard.Begin(element);
			}
			else {
				// Log or break if the resource isn't found.
				System.Diagnostics.Debug.WriteLine($"Resource {animationKey} was not found.");
			}
		}
	}
}
