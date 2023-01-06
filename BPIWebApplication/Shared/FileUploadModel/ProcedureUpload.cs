﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Shared.FileUploadModel
{
    public class ProcedureUpload
    {
        public QueryModel<Procedure> procedureDetails { get; set; } = new QueryModel<Procedure>();
        public List<FileReadyUpload> files { get; set; } = new List<FileReadyUpload>();
    }
}
