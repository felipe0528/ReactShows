import { IActor } from "./actor";
import { ISeason } from "./season";

export interface IShow
{
    id: number;
    idAPI: number;
    name :string;
    photoURL :string;
    channel :string;
    summary:string;
    rating :string;
    genere :string;
    time :string;
    days :string;
    language: string;
    cast : IActor[];
    seasons: ISeason[];
}