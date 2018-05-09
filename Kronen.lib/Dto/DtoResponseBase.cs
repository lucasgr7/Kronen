using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Kronen.lib.Dto
{
    public class DtoResponseBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        protected List<KronenError> Errors {get;set;}
        public bool ExistErrors(){
            return Errors != null && Errors.Any();
        }
        public List<KronenError> getErrors(){
            if(this.Errors == null)
                this.Errors = new List<KronenError>();
            return this.Errors;
        }
        public bool Success { get { return !ExistErrors(); } }
        
        public KronenError AddError<T>(T erro, Exception ex)
            where T : struct
        {
            return AddError(erro, ex.Message);
        }
        public KronenError AddError<T>(T erro, string message)
            where T : struct
        {
            return AddError(erro, string.Empty);
        }

        public KronenError AddError<T>(T erro)
            where T : struct
        {
            return AddError(erro, string.Empty);
        }
        
    }
}