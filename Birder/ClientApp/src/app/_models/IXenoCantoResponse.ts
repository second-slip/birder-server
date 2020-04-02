export interface IXenoCantoResponse {
    numRecordings: string;
    numSpecies: string;
    page: string;
    numPages: string;
    recordings: IVoice[];
  }

  export interface IVoice {
    id: number;
    url: string;
  }
