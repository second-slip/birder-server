import { ConserverationStatus } from './ConserverationStatus';

export interface BirdDetailViewModel {
    BirdId: number;
    Class: string;
    Order: string;
    Family: string;
    Genus: string;
    Species: string;
    EnglishName: string;
    InternationalName: string;
    Category: string;
    PopulationSize: string;
    BtoStatusInBritain: string;
    ThumbnailUrl: string;
    SongUrl: string;
    CreationDate: Date | string;
    LastUpdateDate: Date | string;
    BirdConserverationStatus: ConserverationStatus;
    BirderStatus: string;
}
