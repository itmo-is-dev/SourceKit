## Release 1.0

### New Rules

 Rule ID | Category    | Severity | Notes                                                                    
---------|-------------|----------|--------------------------------------------------------------------------
 SK1000  | Usage       | Error    | Checkes if type is partial when it supposed to be                        
 SK1200  | Usage       | Error    | Null forgiving operator is not allowed                                   
 SK1201  | Usage       | Error    | #nullable disable is not allowed                                         
 SK1300  | Usage       | Error    | When using LINQ's OfType, the casted type must be derived from base type 
 SK1301  | Performance | Error    | Do not chain LINQ methods after collection materialization               
 SK1400  | Design      | Warning  | Use C# properties over fields + getter/setter                            
 SK1500  | Design      | Error    | Type argument for Dictionary's TKey must implement IEquatable            
 SK1501  | Design      | Warning  | Using ForEach method is not allowed                                      