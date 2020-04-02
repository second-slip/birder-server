import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { IXenoCantoResponse } from '@app/_models/IXenoCantoResponse';
import { XenoCantoService } from '@app/xeno-canto.service';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  recordings: IXenoCantoResponse;
  recording: string;
  jimmy: IVoice[];

  constructor(private xeno: XenoCantoService) { }

  ngOnInit() {
    this.xeno.getRecordings('troglodytes troglodytes')
      .subscribe((results: IXenoCantoResponse) => {
        this.recordings = results;
        // console.log(results);
        this.stringProcess(results.recordings);
      });

  }

  getPosition(stringa, subString, index) {
    return stringa.split(subString, index).join(subString).length + 1;
  }

  stringProcess(data) {
    const urls: IVoice[] = [];
    data.length = 10;
    // console.log(data);
    // const parsed = JSON.parse(data);
    // console.log(parsed[0].small);

    // var x = data.map(person => {
    //   console.log(person.file);
    //   console.log(person.sono['full']);
    //   person.sono.map((comment, index) => console.log(index + " " + comment));
    // });

    // const x = '//www.xeno-canto.org/sounds/uploaded/OKHOSXCBNN/ffts/XC539497-full.png';
    // const file = '//www.xeno-canto.org/538352/download';
    // const sub = x.substr(3, 45);
    // alert(sub);
    // const x = '//www.xeno-canto.org/sounds/uploaded/OKHOSXCBNN/ffts/XC539497-full.png';
    // const string = "XYZ 123 ABC 456 ABC 789 ABC";
    // const g = [];
    data.forEach((element, index) => {
      let sub = element.sono['full'].substr(0, this.getPosition(element.sono['full'], '\/', 6));
      urls.push({
        id: index + 1,
        url: `${sub}${element['file-name']}`
      });

    });

    this.jimmy = urls;
    console.log(this.jimmy);
  }
}




// console.log(g);

// const sub = x.substr(0, this.getPosition(x, '\/', 6));

// alert(`${sub}${file}`);
// this.recording = `${sub}${file}`;

// alert(
//   getPosition(x, '\/', 6) // --> 16
// )
// this.xeno.getRecordings('troglodytes troglodytes')
// .subscribe((results: IXenoCantoResponse) => {
//   this.recordings = results;
//   console.log(results);
// });


export interface IVoice {
  id: number;
  url: string;
}
