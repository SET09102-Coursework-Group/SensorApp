using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;
/// <summary>
/// Interface for menu factories that produce lists of FlyoutItems.
/// </summary>
public interface IMenuFactory
{
    IEnumerable<AppMenuItem> CreateMenu();
}