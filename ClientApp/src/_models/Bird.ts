export interface Bird {
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
    CreationDate: Date | string;
    LastUpdateDate: Date | string;
    ConserverationStatusId: number;
    // Observations: Observation[];
    BirdConserverationStatus: ConserverationStatus;
    // BirderStatus: BirderStatus;
    // TweetDay: TweetDay[];
}

export interface ConserverationStatus
{
    ConserverationStatusId: number;

    ConservationStatus: string;

    Description: string;

    CreationDate: Date | string;

    LastUpdateDate: Date | string;

    Birds: Bird[];
}
