export class NoteModel {
    constructor(
      public id: number,
      public noteType: string,
      public note: string,
    ) {  }
    }
  
  // export interface ObservationNote {
  //   id: number;
  //   noteType: ObservationNoteType; ///????
  //   note: string;
  //   obervationId: number;
  //   // observation: ObservationViewModel; ????
  // }