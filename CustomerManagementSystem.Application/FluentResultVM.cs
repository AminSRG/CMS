using System.Collections.Generic;

namespace CustomerManagementSystem.Application
{
    public class FluentResultVM<TValue>
    {
        public bool IsFailed { get; set; }
        public bool IsSuccess { get; set; }
        public List<Reason> reasons { get; set; }
        public List<Error> Errors { get; set; }
        public List<successe> successes { get; set; }
        public TValue valueOrDefault { get; set; }
        public TValue value { get; set; }
    }

    public class FluentResultWithListVM<TValue>
    {
        public bool IsFailed { get; set; }
        public bool IsSuccess { get; set; }
        public List<Reason> reasons { get; set; }
        public List<Error> Errors { get; set; }
        public List<successe> successes { get; set; }
        public List<TValue> valueOrDefault { get; set; }
        public List<TValue> value { get; set; }
    }

    public class Reason
    {
        public string Message { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string AdditionalProp1 { get; set; }
        public string AdditionalProp2 { get; set; }
        public string AdditionalProp3 { get; set; }
    }
    public class Error
    {
        public string Message { get; set; }
        public Metadata Metadata { get; set; }
        public string[] reasons { get; set; }
    }

    public class OtpError
    {
        public int errorCode { get; set; }
        public string errorDescription { get; set; }
        public string referenceName { get; set; }
    }

    public class successe
    {
        public string Message { get; set; }
        public Metadata Metadata { get; set; }
    }
}
