//  ng g interface IFeatures data
//  ng g i <name> <type> [options]

export interface IFeatures {
    id: number;
    feature: string;
    description: string;
    progress: string;
    priority: string;
    colourCode: string;
}