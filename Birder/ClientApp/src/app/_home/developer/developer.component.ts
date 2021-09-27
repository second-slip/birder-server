import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-developer',
  templateUrl: './developer.component.html',
  styleUrls: ['./developer.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DeveloperComponent { }



// export interface Experience {
//   type: string;
//   detail: string;
// }

// const Data: Experience[] = [
//   {type: 'Languages:', detail: 'C#, TypeScript, SQL'},
//   {type: '.NET:', detail: ".NET 5 (Core), ASP.NET, MVC, .NET Framework 4 WPF MVVM, \n Entity Framework/Core (code-first), LINQ, AutoMapper; \n XUnit, Moq"}
// ]
