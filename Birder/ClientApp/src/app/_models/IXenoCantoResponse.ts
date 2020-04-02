export interface IXenoCantoResponse {
    numRecordings: string;
    numSpecies: string;
    page: string;
    numPages: string;
    recordings: IMappedRecordings[];
  }

  export interface IMappedRecordings {
    id: number;
    url: string;
  }
