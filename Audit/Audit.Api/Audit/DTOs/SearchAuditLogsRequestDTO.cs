﻿using Audit.Api.Enums;
using Core.Api.Models;

namespace Audit.Api.Audit.DTOs
{
    public class SearchAuditLogsRequestDTO : SearchRequestDTOBase
    {
        public AuditService? ServiceId { get; set; }
        public AuditEvent? EventId { get; set; }
        public string? EventEntityId { get; set; }
    }
}
