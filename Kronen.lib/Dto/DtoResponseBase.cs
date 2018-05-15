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
        
        public void AddError<T>(T erro, Exception ex)
            where T : struct
        {
            AddError(erro, ex.Message);
        }
        public void AddError<T>(T erro, string message)
            where T : struct
        {
            if(this.Errors == null)
                this.Errors = new List<KronenError>();
            this.Errors.Add(new KronenError(){
                message = message
            });
        }

        public void AddError<T>(T erro)
            where T : struct
        {
            AddError(erro, string.Empty);
        }
        
    }
}