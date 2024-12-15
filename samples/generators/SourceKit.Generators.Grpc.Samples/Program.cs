using System;
using System.Collections.Generic;
using Playground;

Console.WriteLine("Hello, World!");

var model = new ProtoProtoModel(
    pageToken: "",
    values: new[] { "" },
    pageSize: 0,
    intValues: new[] { 0 },
    intOneofValue: null,
    stringOneofValue: null,
    notNullStringValue: "",
    nullIntValue: null,
    nullStringValue: null,
    mapValue: new Dictionary<int, string> { [1] = "1" },
    m: new ProtoProtoModel.Types.InnerMessage(@enum: ProtoProtoModel.Types.InnerEnum.Aboba1));

var emptyModel = new ProtoEmptyMessage();

Console.WriteLine(model);