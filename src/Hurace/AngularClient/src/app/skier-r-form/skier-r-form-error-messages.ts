export class ErrorMessage {
    constructor(
      public forControl: string,
      public forValidator: string,
      public text: string
    ) { }
  }

export const SkierFormErrorMessages = [
    new ErrorMessage('firstName', 'required', 'Ein Vorname muss angegeben werden'),
    new ErrorMessage('lastName', 'required', 'Ein Nachname muss angegeben werden'),
    new ErrorMessage('sex', 'required', 'Es muss ein Geschlecht angegeben werden'),
    new ErrorMessage('year', 'required', 'Es muss ein GeburtsJahr angegeben werden'),
    new ErrorMessage('country', 'required', 'Es muss eine Nationalität angegeben werden')
];
