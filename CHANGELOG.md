# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

## [Unreleased]

## [2.0.0] - 2022-07-23
### Changed
* Upgrade to net6

## [1.1.0] - 2021-07-17
### Added
* CatchItemPropertyChanging for the ObservableList
* CatchItemPropertyChanging and CatchItemPropertyChanged for ObservableDictionary

## [1.0.0] - 2021-06-13
### Added
* EnumerableEx (Extends IEnumerable with some helpers)
* ListEx (Extends the IList with some helpers)
* ObservableDictionary (A dictionary which implements the ICollectionChanged, INotifyPropertyChanging and INotifyPropertyChanged to be able to bind it on the UI)
* ObservableList (A list which implements the ICollectionChanged, INotifyPropertyChanging and INotifyPropertyChanged to be able to bind it on the UI)
