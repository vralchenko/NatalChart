import { createContext, useContext, useState, type ReactNode } from 'react';
import { translations, type Lang, type Translations } from '../i18n';

interface LangContextType {
  lang: Lang;
  setLang: (lang: Lang) => void;
  t: Translations;
}

const LangContext = createContext<LangContextType>({
  lang: 'en',
  setLang: () => {},
  t: translations.en,
});

export const useLang = () => useContext(LangContext);

export const LangProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [lang, setLang] = useState<Lang>(() => {
    const saved = localStorage.getItem('natalchart-lang');
    return (saved === 'ru' || saved === 'en') ? saved : 'en';
  });

  const handleSetLang = (newLang: Lang) => {
    setLang(newLang);
    localStorage.setItem('natalchart-lang', newLang);
  };

  return (
    <LangContext.Provider value={{ lang, setLang: handleSetLang, t: translations[lang] }}>
      {children}
    </LangContext.Provider>
  );
};
