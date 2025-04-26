using SensorApp.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorApp.Shared.Interfaces
{
    /// <summary>
    /// Interface for interacting with the sensor API.
    /// </summary>
    public interface ISensorApiService
    {
        /// <summary>
        /// Fetches a list of sensors from the API using the provided authentication token.
        /// </summary>
        /// <param name="token">The authentication token used for authorization in the request.</param>
        /// <returns>A list of <see cref="SensorModel"/> representing the sensors, or an empty list if an error occurs.</returns>
        Task<List<SensorModel>> GetSensorsAsync(string token);
    }
}
