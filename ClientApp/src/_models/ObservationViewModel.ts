import { Bird } from './Bird';
import { UserViewModel } from './UserViewModel';

export interface ObservationViewModel {
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
    // ApplicationUserId: string; // not required, but do need a vanilla user object
    Bird: Bird;
    // ApplicationUser: ApplicationUser;
    User: UserViewModel;
    // ObservationTags: ObservationTag[];
}
