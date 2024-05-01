## Release 1.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
 SK1300| Usage       | Error    | When using LINQ's OfType, the casted type must be derived from base type 
 SK1301 | Performance | Error    | Do not chain LINQ methods after collection materialization 