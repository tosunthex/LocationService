using System;
using System.Collections.Generic;
using System.Linq;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Persistence
{
    public class InMemeroyLocationRepository:ILocationRecordRepository
    {
        private static Dictionary<Guid, SortedList<long, LocationRecord>> locationRecords;

        public InMemeroyLocationRepository()
        {
            if(locationRecords == null)
            {
                locationRecords = new Dictionary<Guid,SortedList<long,LocationRecord>>();
            }
        }
        public LocationRecord Add(LocationRecord locationRecord)
        {
            var memberRecords = GetMemberRecords(locationRecord.MemberID);
            memberRecords.Add(locationRecord.Timestamp,locationRecord);
            locationRecords.Add(locationRecord.ID,memberRecords);
            return locationRecord;
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            Delete(locationRecord.MemberID, locationRecord.ID);
            Add(locationRecord);
            return locationRecord;
        }

        public LocationRecord Get(Guid memberId, Guid recordId)
        {
            return GetMemberRecords(memberId).Values.FirstOrDefault(lr => lr.ID == recordId);
        }

        public LocationRecord Delete(Guid memberId, Guid locationId)
        {
            var memberRecords = GetMemberRecords(memberId);
            var lr = memberRecords.Values.FirstOrDefault(mr => mr.ID == locationId);
            if (lr != null)
                memberRecords.Remove(lr.Timestamp);

            return lr;
        }

        public LocationRecord GetLatestForMember(Guid memberId)
        {
            return GetMemberRecords(memberId).Values.LastOrDefault();
        }

        public ICollection<LocationRecord> AllForMember(Guid memberId)
        {
            var memberRecords = GetMemberRecords(memberId);
            return memberRecords.Values.Where(mr => mr.MemberID == memberId).ToList();
        }
        
        private SortedList<long, LocationRecord> GetMemberRecords(Guid memberId) {
            if(!locationRecords.ContainsKey(memberId))
                locationRecords.Add(memberId,new SortedList<long, LocationRecord>());
                
            return locationRecords[memberId];
        }
    }
}