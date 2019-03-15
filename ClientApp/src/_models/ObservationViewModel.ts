import { UserViewModel } from './UserViewModel';
import { BirdSummaryViewModel } from './BirdSummaryViewModel';

export interface ObservationViewModel {
   [x: string]: any;
    observationId: number;
    locationLatitude: number;
    locationLongitude: number;
    quantity: number;
    noteGeneral: string;
    noteHabitat: string;
    noteWeather: string;
    noteAppearance: string;
    noteBehaviour: string;
    noteVocalisation: string;
    hasPhotos: boolean;
    // SelectedPrivacyLevel: PrivacyLevel;
    observationDateTime: Date | string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birdId: number;
    // ApplicationUserId: string; // not required, but do need a vanilla user object
    bird: BirdSummaryViewModel;
    // ApplicationUser: ApplicationUser;
    user: UserViewModel;
    // ObservationTags: ObservationTag[];
}
