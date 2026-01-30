using GarageManagement.Server.dtos;
using Microsoft.AspNetCore.Mvc;

namespace GarageManagement.Server.RepoInterfaces
{
    public interface IAutoCompleteRepository
    {
        Task<IList<IdNameDto>> SearchRegistration(string query);
        Task<IList<IdNameDto>> SearchJobCradsForEstimation(string query);
        Task<IList<IdNameDto>> SearchBookingCustomer(string query);
        Task<IList<IdNameDto>> SearchBookingAppointment(string query);
        Task<IList<IdNameDto>> SearchRepairOrder(string query);
    }
}
