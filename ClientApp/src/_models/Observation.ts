import { Bird } from './Bird';

export interface Observation {
    ObservationId: number;
    LocationLatitude: number;
    LocationLongitude: number;
    Quantity: number;
    NoteGeneral: string;
    NoteHabitat: string;
    NoteWeather: string;
    NoteAppearance: string;
    NoteBehaviour: string;
    NoteVocalisation: string;
    HasPhotos: boolean;
    // SelectedPrivacyLevel: PrivacyLevel;
    ObservationDateTime: Date | string;
    CreationDate: Date | string;
    LastUpdateDate: Date | string;
    BirdId: number;
    ApplicationUserId: string;
    Bird: Bird;
    // ApplicationUser: ApplicationUser;
    // ObservationTags: ObservationTag[];
}
