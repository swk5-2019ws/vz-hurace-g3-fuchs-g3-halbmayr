import { Skier, AssociatedOfStartPosition, AssociatedOfCountry, AssociatedOfSex, Sex, Country } from "../converter.client";

export class SkierClass implements Skier {
  constructor(
  firstName?: string,
  lastName?: string,
  dateOfBirth?: Date,
  imageUrl?: string,
  isRemoved?: boolean,
  startPositions?: AssociatedOfStartPosition[],
  country?: AssociatedOfCountry,
  sex?: AssociatedOfSex) {}
}

class AssociatedOfCountryClass implements AssociatedOfCountry{
  foreignKey?: number | undefined;
  reference?: Country | undefined;
  initialised?: boolean;
}

class AssociatedOfSexClass implements AssociatedOfSex{
  foreignKey?: number | undefined;
  reference?: Sex | undefined;
  initialised?: boolean;
}
