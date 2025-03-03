using System;

namespace DotNetPoC.Functions.Models;

internal record AuditTrailData(
  Guid Id,
  string Message,
  DateTime CreatedAt
);

