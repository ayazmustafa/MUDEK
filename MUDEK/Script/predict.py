import sys
import nltk
import os
from sklearn.feature_extraction.text import TfidfVectorizer
import pickle


import nltk
from nltk.corpus import stopwords
from snowballstemmer import TurkishStemmer
from nltk.stem.snowball import SnowballStemmer

def preprocess(sentence,language,stem=True):
    sentence = sentence.lower()
    tokens = nltk.word_tokenize(sentence)
    stop_words = stopwords.words(language)
    tokens = [w for w in tokens if not w in stop_words and w.isalpha()]
    if stem:
        if(language == 'turkish'):
            stemmer = TurkishStemmer()
            tokens = [stemmer.stemWord(word) for word in tokens]
        else:
            stemmer = SnowballStemmer(language)
            tokens = [stemmer.stem(word) for word in tokens]
    return tokens


# Install NLTK dependencies
nltk.download('stopwords')
nltk.download('punkt')

# Read input data
course_name = sys.argv[1]
language = sys.argv[2]
amount = int(sys.argv[3])
questions = []
for i in range(4,amount+4):
    question = sys.argv[i]
    questions.append(question)

# Load model and vectorizer
filename = course_name + '.sav'
clf,vect = pickle.load(open("Script/BP1.sav", 'rb'))

# Preprocess
questions = [' '.join(preprocess(text,language,stem=False)) for text in questions]

# Prediction
predictions = []
for sentence in questions:
    sentence_counts = vect.transform([sentence])
    prediction = clf.predict(sentence_counts)
    predictions.append(prediction[0])
predictions = '|'.join(predictions)

# Return results
# print(predictions)
sys.stdout.write(predictions)