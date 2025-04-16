using SensorApp.Shared.Models;

namespace SensorApp.Shared.Interfaces;

public interface IMenuBuilder
{
    void BuildMenu(UserInfo userInfo);
}
