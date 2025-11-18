
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Attendance_RecordService
    {
        Task<WebResponseContent> GetRecordList(PageInput<AttendanceRecordQuery> query);
        Task<WebResponseContent> GetRecord(int userId, string userNo, string punchTime);
        Task<WebResponseContent> AddData(AttendanceRecordDto recordDto);

        Task<AttendanceRecordWebResponseDto> ClockIn(AttendanceRecordWebDto webDto);

        /// <summary>
        /// ����ʱ���ȡ�������ڴ��û��Լ�����������ʱ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetMaxAndMinTimeRecordListAsync(AtdRecordMaxMinTimeQuery input);
        
        
        /// <summary>
        ///  某天打卡最早和最晚时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDayRecordDetailsListAsync(AtdDayRecordDetailsQuery input);
    }
 }
