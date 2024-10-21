module Startup

open Microsoft.Extensions.Configuration

let configuration = ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
