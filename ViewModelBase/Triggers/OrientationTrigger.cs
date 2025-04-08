//------------------------------------------------------------
// <copyright file="OrientationTrigger.cs" company="CyberFeedForward" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.ViewModelBase.Triggers;

using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;

class OrientationTrigger : StateTriggerBase
{
    public ApplicationViewOrientation Orientation { get; set; }

    public OrientationTrigger()
    {
        Window.Current.SizeChanged += (s, e) =>
        {
            SetActive(ApplicationView.GetForCurrentView().Orientation.Equals(Orientation));
        };
    }
}
