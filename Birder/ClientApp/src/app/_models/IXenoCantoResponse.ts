export interface IXenoCantoResponse {
    numRecordings: string;
    numSpecies: string;
    page: string;
    numPages: string;
    recordings: IRecording[];
  }

  export interface IRecording {
    id: number;
    url: string;
  }
