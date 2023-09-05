// Test specific
global using Moq;
global using Xunit;
global using Birder.TestsHelpers;
//global using TestSupport.EfHelpers;  --> removed this global to make it explicit where this library is used
global using Xunit.Extensions.AssertExtensions;
global using FluentAssertions;

global using System;
global using System.Text;
global using System.Linq;
global using System.Collections.Generic;
global using System.Linq.Expressions;
global using System.Threading.Tasks;

// MVC related libraries
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Logging;

// project specific
global using Birder.Data;
global using Birder.Helpers;
global using Birder.Services;
global using Birder.ViewModels;
global using Birder.Controllers;
global using Birder.Data.Repository;
global using Birder.Data.Model;
global using Birder.Infrastructure.Configuration;