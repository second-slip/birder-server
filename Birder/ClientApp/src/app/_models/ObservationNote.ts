import { ObservationViewModel } from './ObservationViewModel';

export enum ObservationNoteType {
    General = 0,
    Habitat = 1,
    Weather = 2,
    Appearance = 3,
    Behaviour = 4,
    Vocalisation = 5
}

export interface ObservationNote {
    id: number;
    noteType: string; ///????
    note: string;
    obervationId: number;
    // observation: ObservationViewModel; ????
}